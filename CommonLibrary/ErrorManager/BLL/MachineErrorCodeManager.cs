using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineErrorCodeManager
    {
        MachineErrorCodeService machineErrorCodeService = new MachineErrorCodeService();

        /// <summary>
        /// 查询所有error_code_details
        /// </summary>
        /// <returns></returns>
        public List<MachineErrorCode> GetAllErrorCode()
        {
            return machineErrorCodeService.GetAllErrorCode();
        }

        /// <summary>
        /// 查询指定编码的error_code_details
        /// </summary>
        /// <returns></returns>
        public MachineErrorCode GetErrorCodeByNo(string code_no)
        {
            return machineErrorCodeService.GetErrorCodeByNo(code_no);
        }
        /// <summary>
        /// 查询指定编码的error_code
        /// </summary>
        /// <returns></returns>
        public MachineErrorCode GetErrorCodeByValue(string tag_value)
        {
            return machineErrorCodeService.GetErrorCodeByValue(tag_value);
        }
        /// <summary>
        /// 查询指定编码的error_code_details
        /// </summary>
        /// <returns></returns>
        public MachineErrorCode GetErrorCodeById(int id)
        {
            return machineErrorCodeService.GetErrorCodeById(id);
        }

        public List<MachineErrorCode> SelectAll(string code_no)
        {
            return machineErrorCodeService.SelectAll(code_no);
        }

        public MachineErrorCode SelectSingle(string code_no, int id)
        {
            return machineErrorCodeService.SelectSingle(code_no, id);
        }

        public int Insert(MachineErrorCode obj)
        {
            return machineErrorCodeService.Insert(obj);
        }

        public int Update(MachineErrorCode obj)
        {
            return machineErrorCodeService.Update(obj);
        }

        public int Del(int id)
        {
            return machineErrorCodeService.Del(id);
        }
    }
}
