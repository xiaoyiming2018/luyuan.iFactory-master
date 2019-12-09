using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonHelper.ScadaAPI
{
    class TokenResult
    {
        public string tokenType { get; set; }
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string refreshToken { get; set; }
    }

    class RealData
    {
        public string scadaId { get; set; }
        public string deviceId { get; set; }
        public int index { get; set; }
        public string value { get; set; }
        public string tagName { get; set; }
    }
}
