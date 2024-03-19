
using AdGrowth.Interfaces;

namespace AdGrowth.Exceptions
{

    public class AdRequestException
    {

        public const string PLAYBACK_ERROR = "playback_error";
        public const string NETWORK_ERROR = "network_error";
        public const string UNKNOWN_ERROR = "unknown_error";
        public const string UNIT_ID_MISMATCHED_AD_TYPE = "unit_id_mismatched_ad_type";
        public const string INTERNAL_ERROR = "internal_error";
        public const string INVALID_CLIENT_KEY = "invalid_client_key";
        public const string INVALID_UNIT_ID = "invalid_unit_id";
        public const string SDK_NOT_INITIALIZED = "sdk_not_initialized";
        public const string NO_AD_FOUND = "no_ad_found";
        public const string ALREADY_LOADED = "already_loaded";
        public const string ALREADY_CONSUMED = "already_consumed";
        public const string NOT_READY = "not_ready";
        public const string ALREADY_SHOWING_FULL_SCREEN_AD = "already_showing_full_screen_ad";

        public readonly string code;
        public readonly object message = "";

        public AdRequestException(string code,string message)
        {
            this.code = code;
            this.message = message;
        }

    }
}
