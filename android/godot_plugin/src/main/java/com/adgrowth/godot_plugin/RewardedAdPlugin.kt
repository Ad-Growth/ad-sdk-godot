package com.adgrowth.godot_plugin

import android.util.ArraySet
import com.adgrowth.adserver.RewardedAd
import com.adgrowth.adserver.entities.RewardItem
import com.adgrowth.adserver.exceptions.AdRequestException
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

import org.godotengine.godot.Dictionary
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.SignalInfo
import org.godotengine.godot.plugin.UsedByGodot


class RewardedAdPlugin(godot: Godot?) : GodotPlugin(godot) {
    private val mainScope = CoroutineScope(Dispatchers.Main)
    private val mAds: HashMap<String, RewardedAd> = HashMap()

    override fun getPluginName() = BuildConfig.REWARDED_AD_PLUGIN_NAME

    override fun getPluginSignals(): Set<SignalInfo> {
        val signals = ArraySet<SignalInfo>()
        signals.add(SignalInfo("onLoad", String::class.java))
        signals.add(SignalInfo("onFailedToLoad", String::class.java, Dictionary::class.java))
        signals.add(SignalInfo("onClicked", String::class.java))
        signals.add(SignalInfo("onDismissed", String::class.java))
        signals.add(SignalInfo("onFailedToShow", String::class.java, String::class.java))
        signals.add(SignalInfo("onImpression", String::class.java))
        signals.add(SignalInfo("onEarnedReward", String::class.java, Dictionary::class.java))
        return signals
    }

    @UsedByGodot
    fun show(instanceId: String) {
        mainScope.launch {
            mAds[instanceId]?.show(activity!!)
        }
    }

    @UsedByGodot
    fun getInstance(unitId: String): String {
        val ad = RewardedAd(unitId)
        val instanceId = ad.hashCode().toString()

        mAds[instanceId] = ad

        return instanceId
    }

    @UsedByGodot
    fun load(instanceId: String) {
        mAds[instanceId]?.setListener(object : RewardedAd.Listener {
            override fun onLoad(ad: RewardedAd) {
                emitSignal("onLoad", instanceId)
            }

            override fun onFailedToLoad(exception: AdRequestException?) {
                val map = Dictionary()

                map["code"] = exception?.code ?: AdRequestException.UNKNOWN_ERROR
                map["message"] = exception?.message ?: map["code"]

                emitSignal("onFailedToLoad", instanceId, map)
            }

            override fun onEarnedReward(rewardItem: RewardItem) {
                val map = Dictionary()

                map["item"] = rewardItem.item
                map["value"] = rewardItem.value

                emitSignal("onEarnedReward", instanceId, map)
            }

            override fun onClicked() {
                emitSignal("onClicked", instanceId)
            }

            override fun onDismissed() {
                mAds.remove(instanceId)
                emitSignal("onDismissed", instanceId)
            }

            override fun onFailedToShow(code: String?) {
                emitSignal("onFailedToShow", instanceId, code)
            }

            override fun onImpression() {
                emitSignal("onImpression", instanceId)
            }
        })

        mAds[instanceId]?.load(activity!!)
    }

    @UsedByGodot
    fun isLoaded(instanceId: String): Boolean {
        return mAds[instanceId]?.isLoaded == true
    }

    @UsedByGodot
    fun isFailed(instanceId: String): Boolean {
        return mAds[instanceId]?.isFailed == true
    }
}