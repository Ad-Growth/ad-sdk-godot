package com.adgrowth.godot_plugin

import android.util.ArraySet
import com.adgrowth.adserver.AdServer
import com.adgrowth.adserver.entities.ClientProfile

import com.adgrowth.adserver.exceptions.SDKInitException
import org.godotengine.godot.Dictionary
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.SignalInfo
import org.godotengine.godot.plugin.UsedByGodot

class AdServerPlugin(godot: Godot?) : GodotPlugin(godot) {

    override fun getPluginName() = BuildConfig.ADSERVER_PLUGIN_NAME

    override fun getPluginSignals(): Set<SignalInfo> {
        val signals = ArraySet<SignalInfo>()
        signals.add(SignalInfo("onInit"))
        signals.add(SignalInfo("onFailed", SDKInitException::class.java))
        return signals
    }

    @UsedByGodot
    fun isInitialized(): Boolean {
        return AdServer.isInitialized()
    }

    @UsedByGodot
    fun initialize() {
        val activity = activity!!
        AdServer.initialize(activity, object : AdServer.Listener {
            override fun onInit() {
                emitSignal("onInit")
            }

            override fun onFailed(e: SDKInitException?) {
                emitSignal("onFailed", e)
            }
        })
    }

    @UsedByGodot
    fun getClientProfile(): Dictionary {
        val clientProfile = AdServer.getClientProfile()
        val map = Dictionary()
        map["gender"] = clientProfile.gender.toString()
        map["age"] = clientProfile.age.toString()
        map["maxAge"] = clientProfile.maxAge.toString()
        map["minAge"] = clientProfile.minAge.toString()
        return map
    }

    @UsedByGodot
    fun getClientAddress(): Dictionary {
        val clientAddress = AdServer.getClientProfile().clientAddress
        val map = Dictionary()
        map["country"] = clientAddress.country
        map["state"] = clientAddress.state
        map["city"] = clientAddress.city
        map["latitude"] = clientAddress.latitude
        map["longitude"] = clientAddress.longitude
        return map
    }

    @UsedByGodot
    fun setClientAddressField(field: String, value: String) {
        val clientAddress = AdServer.getClientProfile().clientAddress

        when (field) {
            "country" -> clientAddress.country = value
            "state" -> clientAddress.state = value
            "city" -> clientAddress.city = value
            "latitude" -> clientAddress.latitude = value.toDouble()
            "longitude" -> clientAddress.longitude = value.toDouble()
        }
    }

    @UsedByGodot
    fun getInterests(): Array<String> {
        return AdServer.getClientProfile().interests.toTypedArray()
    }

    @UsedByGodot
    fun setClientProfileField(field: String, value: String) {
        val clientProfile = AdServer.getClientProfile()

        when (field) {
            "gender" -> clientProfile.gender = ClientProfile.Gender.valueOf(value)
            "age" -> clientProfile.age = value.toInt()
            "maxAge" -> clientProfile.maxAge = value.toInt()
            "minAge" -> clientProfile.minAge = value.toInt()
        }
    }

    @UsedByGodot
    fun addInterest(value: String) {
        AdServer.getClientProfile().addInterest(value)
    }

    @UsedByGodot
    fun removeInterest(value: String) {
        AdServer.getClientProfile().removeInterest(value)
    }

}