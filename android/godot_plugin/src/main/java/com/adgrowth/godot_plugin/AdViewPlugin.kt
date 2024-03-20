package com.adgrowth.godot_plugin

import android.content.res.Resources
import android.os.Build
import android.util.ArraySet
import android.view.Gravity
import android.view.ViewGroup
import android.widget.FrameLayout.LayoutParams
import com.adgrowth.adserver.views.AdView
import com.adgrowth.adserver.enums.AdOrientation
import com.adgrowth.adserver.enums.AdSize
import com.adgrowth.adserver.exceptions.AdRequestException
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

import org.godotengine.godot.Dictionary
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.SignalInfo
import org.godotengine.godot.plugin.UsedByGodot
import java.lang.Exception
import kotlin.math.roundToInt


class AdViewPlugin(godot: Godot?) : GodotPlugin(godot) {
    private val mainScope = CoroutineScope(Dispatchers.Main)
    private val mAds: HashMap<String, AdView> = HashMap()
    private val mAdSettings: HashMap<String, MutableMap<String, Any>> = HashMap()
    private val mDisplayMetrics = Resources.getSystem().displayMetrics
    private val mEdgeInsets: Map<String, Int>
        get() {
            var top = 0
            var right = 0
            var bottom = 0
            var left = 0

            try {
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
                    val displayCutout =
                        activity!!.window.decorView.rootView.rootWindowInsets.displayCutout
                    top = displayCutout?.safeInsetTop ?: 0
                    right = displayCutout?.safeInsetRight ?: 0
                    bottom = displayCutout?.safeInsetBottom ?: 0
                    left = displayCutout?.safeInsetLeft ?: 0
                }
            } catch (_: Exception) {
            }

            return mapOf("top" to top, "right" to right, "bottom" to bottom, "left" to left)
        }

    override fun getPluginName() = BuildConfig.ADVIEW_PLUGIN_NAME

    override fun getPluginSignals(): Set<SignalInfo> {
        val signals = ArraySet<SignalInfo>()
        signals.add(SignalInfo("onLoad", String::class.java))
        signals.add(SignalInfo("onFailedToLoad", String::class.java, Dictionary::class.java))
        signals.add(SignalInfo("onClicked", String::class.java))
        signals.add(SignalInfo("onDismissed", String::class.java))
        signals.add(SignalInfo("onFailedToShow", String::class.java, String::class.java))
        signals.add(SignalInfo("onImpression", String::class.java))
        return signals
    }


    @UsedByGodot
    fun getInstance(
        unitId: String, adSize: String, adOrientation: String, position: String
    ): String {
        val ad =
            AdView(activity!!, unitId, AdSize.valueOf(adSize), AdOrientation.valueOf(adOrientation))

        val instanceId = ad.hashCode().toString()

        mAds[instanceId] = ad
        mAdSettings[instanceId] = mutableMapOf("useSafeAreaInsets" to false)

        setLayoutParams(instanceId, adSize, adOrientation)
        setPosition(instanceId, position)

        return instanceId
    }

    // Godot does not support overloads
    @UsedByGodot
    fun getInstanceXY(
        unitId: String, adSize: String, adOrientation: String, x: Int, y: Int
    ): String {
        val ad =
            AdView(activity!!, unitId, AdSize.valueOf(adSize), AdOrientation.valueOf(adOrientation))

        val instanceId = ad.hashCode().toString()

        mAds[instanceId] = ad
        mAdSettings[instanceId] = mutableMapOf("useSafeAreaInsets" to false)
        setLayoutParams(instanceId, adSize, adOrientation)

        setPositionXY(instanceId, x, y)
        return instanceId
    }


    private fun getDPSize(adSize: String): Map<String, Int> {
        return when (AdSize.valueOf(adSize)) {
            AdSize.FULL_BANNER -> mapOf(
                "width" to FULL_BANNER_WIDTH, "height" to FULL_BANNER_HEIGHT
            )

            AdSize.LARGE_BANNER -> mapOf(
                "width" to LARGE_BANNER_WIDTH, "height" to LARGE_BANNER_HEIGHT
            )

            AdSize.LEADERBOARD -> mapOf(
                "width" to LEADERBOARD_WIDTH, "height" to LEADERBOARD_HEIGHT
            )

            AdSize.MEDIUM_RECTANGLE -> mapOf(
                "width" to MEDIUM_RECTANGLE_WIDTH, "height" to MEDIUM_RECTANGLE_HEIGHT
            )

            else -> mapOf(
                "width" to BANNER_WIDTH, "height" to BANNER_HEIGHT
            )
        }
    }

    private fun setLayoutParams(instanceId: String, adSize: String, adOrientation: String) {
        if (mAds[instanceId] == null) return
        val ad = mAds[instanceId]!!
        val size = getDPSize(adSize)

        val density = mDisplayMetrics.density

        if (AdOrientation.valueOf(adOrientation) == AdOrientation.PORTRAIT) {
            ad.layoutParams = LayoutParams(
                (size["height"]!!.times(density)).roundToInt(),
                (size["width"]!!.times(density)).roundToInt()
            )
            return
        }

        ad.layoutParams = LayoutParams(
            (size["width"]!!.times(density)).roundToInt(),
            (size["height"]!!.times(density)).roundToInt()
        )
    }

    @UsedByGodot
    fun setPosition(instanceId: String, adPosition: String) {
        if (mAds[instanceId] == null) return

        val ad = mAds[instanceId]!!
        val settings = mAdSettings[instanceId]!!
        val layoutParams = ad.layoutParams as LayoutParams

        layoutParams.gravity = when (adPosition) {
            "TOP_LEFT" -> Gravity.START
            "TOP_CENTER" -> Gravity.CENTER_HORIZONTAL
            "TOP_RIGHT" -> Gravity.END
            "CENTER_LEFT" -> Gravity.START or Gravity.CENTER_VERTICAL
            "CENTER" -> Gravity.CENTER
            "CENTER_RIGHT" -> Gravity.END or Gravity.CENTER_VERTICAL
            "BOTTOM_LEFT" -> Gravity.START or Gravity.BOTTOM
            "BOTTOM_CENTER" -> Gravity.CENTER or Gravity.BOTTOM
            else -> Gravity.END or Gravity.BOTTOM
        }

        if (settings["useSafeAreaSettings"] == true) {
            layoutParams.topMargin = mEdgeInsets["top"]!!
            layoutParams.rightMargin = mEdgeInsets["right"]!!
            layoutParams.bottomMargin = mEdgeInsets["bottom"]!!
            layoutParams.leftMargin = mEdgeInsets["left"]!!
        }
    }

    @UsedByGodot
    fun setPositionXY(instanceId: String, x: Int, y: Int) {

        if (mAds[instanceId] == null) return

        val settings = mAdSettings[instanceId]!!
        val layoutParams = (mAds[instanceId]!!.layoutParams as LayoutParams)

        layoutParams.gravity = Gravity.NO_GRAVITY

        // use x and y for position
        if (settings["useSafeAreaInsets"] == true) {
            layoutParams.rightMargin = mEdgeInsets["right"]!!
            layoutParams.topMargin = y + mEdgeInsets["top"]!!
            layoutParams.bottomMargin = mEdgeInsets["bottom"]!!
            layoutParams.leftMargin = x + mEdgeInsets["left"]!!
        } else {
            layoutParams.leftMargin = x
            layoutParams.topMargin = y
        }
    }

    @UsedByGodot
    fun enableSafeArea(instanceId: String, enable: Boolean) {
        mAdSettings[instanceId]!!["useSafeAreaInsets"] = enable
    }

    @UsedByGodot
    fun load(instanceId: String) {
        if (mAds[instanceId] == null) return
        val ad = mAds[instanceId]!!

        ad.setListener(object : AdView.Listener {
            override fun onLoad(ad: AdView) {
                emitSignal("onLoad", instanceId)
            }

            override fun onFailedToLoad(exception: AdRequestException?) {
                val map = Dictionary()

                map["code"] = exception?.code ?: AdRequestException.UNKNOWN_ERROR
                map["message"] = exception?.message ?: map["code"]

                emitSignal("onFailedToLoad", instanceId, map)
            }

            override fun onClicked() {
                emitSignal("onClicked", instanceId)
            }

            override fun onDismissed() {
                emitSignal("onDismissed", instanceId)
            }

            override fun onFailedToShow(code: String?) {
                emitSignal("onFailedToShow", instanceId, code)
            }

            override fun onImpression() {
                emitSignal("onImpression", instanceId)
            }
        })

        mainScope.launch {
            if (activity!!.window.decorView is ViewGroup) {
                val view = activity!!.window.decorView as ViewGroup
                view.addView(ad)
            } else activity!!.window.addContentView(ad, ad.layoutParams)
        }
    }

    @UsedByGodot
    fun isLoaded(instanceId: String): Boolean {
        return mAds[instanceId]?.isLoaded == true
    }

    @UsedByGodot
    fun reload(instanceId: String) {
        mAds[instanceId]?.reload()
    }

    @UsedByGodot
    fun destroy(instanceId: String) {
        if (mAds[instanceId] == null) return

        val ad = mAds[instanceId]!!

        if (ad.parent != null) {
            (ad.parent as ViewGroup).removeView(ad)
        }
        mAds.remove(instanceId)
        mAdSettings.remove(instanceId)
    }

    @UsedByGodot
    fun isFailed(instanceId: String): Boolean {
        return mAds[instanceId]?.isFailed == true
    }

    companion object {
        const val BANNER_WIDTH = 320
        const val BANNER_HEIGHT = 50

        const val FULL_BANNER_WIDTH = 468
        const val FULL_BANNER_HEIGHT = 60

        const val LARGE_BANNER_WIDTH = 320
        const val LARGE_BANNER_HEIGHT = 100

        const val LEADERBOARD_WIDTH = 728
        const val LEADERBOARD_HEIGHT = 90

        const val MEDIUM_RECTANGLE_WIDTH = 300
        const val MEDIUM_RECTANGLE_HEIGHT = 250
    }

}