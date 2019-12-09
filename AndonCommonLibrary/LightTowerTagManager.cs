using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using System.Collections.Generic;
using Advantech.IFactory.CommonHelper.ScadaAPI;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Advantech.IFactory.Andon
{
    public class LightTowerTagManager
    {
        private TagService tagService = new TagService();
        private StationManager stationManager = new StationManager();
        private ErrorLogManager errorLogManager = new ErrorLogManager();

        /// <summary>
        /// 当前所有站位的队列，站位是配置完成后不变的信息
        /// </summary>
        public List<station_info> StationsList;//初始化加载所有的站位队列

        public LightTowerTagManager()
        {
            StationsList = stationManager.SelectAll();
        }
        /// <summary>
        /// 获取输出的标签
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="ObjectId">ID</param>
        /// <param name="OutTypevalue"></param>
        /// <returns></returns>
        public T GetLightTowerOutTag<T>(int ObjectId, int OutTypevalue, TagAreaAttributeEnum tagAreaAttributeEnum) where T : new()
        {
            List<T> list = null;
            T TagInfo = default(T);
            
            switch (OutTypevalue)
            {
                case (int)LightTowerEnum.red_light_on:
                    list = tagService.GetWaTagAndTypeInfo<T>(tagAreaAttributeEnum, ObjectId, SystemTagCodeEnum.red_light_on.ToString());
                    break;
                case (int)LightTowerEnum.yellow_light_on:
                    list = tagService.GetWaTagAndTypeInfo<T>(tagAreaAttributeEnum, ObjectId, SystemTagCodeEnum.yellow_light_on.ToString());
                    break;
                case (int)LightTowerEnum.green_light_on:
                    list = tagService.GetWaTagAndTypeInfo<T>(tagAreaAttributeEnum, ObjectId, SystemTagCodeEnum.green_light_on.ToString());
                    break;
                case (int)LightTowerEnum.blue_light_on:
                    list = tagService.GetWaTagAndTypeInfo<T>(tagAreaAttributeEnum, ObjectId, SystemTagCodeEnum.blue_light_on.ToString());
                    break;
                case (int)LightTowerEnum.white_light_on:
                    list = tagService.GetWaTagAndTypeInfo<T>(tagAreaAttributeEnum, ObjectId, SystemTagCodeEnum.white_light_on.ToString());
                    break;
            }
            if (list != null && list.Count > 0)
            {
                TagInfo = list[0];
                return TagInfo;
            }
            return TagInfo;
        }
        /// <summary>
        /// 灯颜色输出写值
        /// </summary>
        /// <param name="device_id"></param>
        /// <param name="ColorValue"></param>
        /// <param name="tagAreaAttributeEnum"></param>
        public bool WriteLightColorAsync(int device_id, int ColorValue, TagAreaAttributeEnum tagAreaAttributeEnum, int Value)
        {
            bool bit = false;
            if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)
            {
                MachineTagInfo tagInfo = GetLightTowerOutTag<MachineTagInfo>(device_id, ColorValue, tagAreaAttributeEnum);
                if (tagInfo != null)
                {
                    if(ScadaAPIConfig.EnableScadaApi)
                    {
                        bit=ScadaAPIHelper.WriteValueAsync(tagInfo.tag_code, Value);
                        if(bit==false)
                        {
                            bit =ScadaAPIHelper.WriteValueAsync(tagInfo.tag_code, Value);//失败再次写入
                        }
                    }
                    else
                    {
                        //MqttManager.MqttHelper.WriteValueToWA(tagInfo.tag_code, Value);//写值
                    }
                }
            }
            else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                try
                {
                    StationTagInfo tagInfo = GetLightTowerOutTag<StationTagInfo>(device_id, ColorValue, tagAreaAttributeEnum);
                    if (tagInfo != null)
                    {
                        if (ScadaAPIConfig.EnableScadaApi)
                        {
                            bit = ScadaAPIHelper.WriteValueAsync(tagInfo.tag_code, Value);
                            if (bit == false)
                            {
                                bit = ScadaAPIHelper.WriteValueAsync(tagInfo.tag_code, Value);//失败再次写入
                            }
                        }
                        else
                        {
                            //MqttManager.MqttHelper.WriteValueToWA(tagInfo.tag_code, Value);//写值
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    bit = false;
                }
            }
            return bit;
        }

        /// <summary>
        /// 现场正常状态下绿灯输出
        /// </summary>
        /// <param name="tagAreaAttributeEnum"></param>
        public bool GreenStatusLightOutAsync(int device_id,TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            List<error_log> list;
            bool bit = false;

            list = errorLogManager.GetAllUnfinishedByDeviceId(device_id);
            if(list!=null && list.Count>0)//有异常，绿灯熄灭
            {
                bit = WriteLightColorAsync(device_id, (int)LightTowerEnum.green_light_on, tagAreaAttributeEnum, 0);
            }
            else
            {
                bit = WriteLightColorAsync(device_id, (int)LightTowerEnum.green_light_on, tagAreaAttributeEnum, 1);
            }
            return bit;
        }
    }
}
