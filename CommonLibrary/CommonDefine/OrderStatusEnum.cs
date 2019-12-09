using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 工单的当前状态
    /// </summary>
    public enum OrderStatusEnum:int
    {
        /// <summary>
        /// 已创建的
        /// </summary>
        Created = 0,
        /// <summary>
        /// 已排产的，列入计划，未执行的
        /// </summary>
        Scheduled = 1,
        /// <summary>
        /// 执行中
        /// </summary>
        Excuting = 2,
        /// <summary>
        /// 正常完成
        /// </summary>
        Finished=3,
        /// <summary>
        /// 取消的
        /// </summary>
        Aborted=4
    }
    /// <summary>
    /// 流程模式
    /// </summary>
    public enum FlowModeEnum
    {
        /// <summary>
        /// 连续模式，即前后站是连续的，中间没有缓存的
        /// </summary>
        Continuous =0,
        /// <summary>
        /// 离散模式，前后站中间有缓存，出来的物料不会立刻流转到下一站
        /// </summary>
        Discrete=1
    }
    /// <summary>
    /// 工单结束模式
    /// </summary>
    public enum OrderFinishMode:int
    {
        /// <summary>
        /// 手工确认结束
        /// </summary>
        ManualFinish =0,
        /// <summary>
        /// 到达数量后自动结束
        /// </summary>
        AutoFinish=1
    }
}
