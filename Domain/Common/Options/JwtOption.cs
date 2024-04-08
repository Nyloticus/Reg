using System;

namespace Common.Options
{
    public class JwtOption
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public TimeSpan TokenLifetime { get; set; }
        public int ExpireDays { get; set; }

    }
}