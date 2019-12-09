using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ClientConfigInfoManager
    {
        client_config_infoService CCIS = new client_config_infoService();

        public client_config_info SelectSingle(string client_mac)
        {
            client_config_info obj = CCIS.SelectSingle(client_mac);
            return obj;
        }

        public int Insert(client_config_info obj)
        {
            int count = CCIS.Insert(obj);
            return count;
        }
        public int Update(client_config_info obj)
        {
            int count = CCIS.Update(obj);
            return count;
        }
    }
}
