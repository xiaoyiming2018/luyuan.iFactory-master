using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 用户权限等级
    /// </summary>
    public enum UserLevelEnum:int
    {
        /// <summary>
        /// 操作员
        /// </summary>
        Operator =0,
        /// <summary>
        /// 管理者
        /// </summary>
        Manager=1,
        /// <summary>
        /// 系统管理员
        /// </summary>
        Admin= 2
    }
}
