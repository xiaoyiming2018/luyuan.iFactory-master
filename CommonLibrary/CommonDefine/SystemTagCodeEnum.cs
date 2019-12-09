using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 系统标签枚举
    /// </summary>
    public enum SystemTagCodeEnum
    {
        //---------------------------------------------------------
        #region 稼动率类别
    
        /// <summary>
        /// 设备运行
        /// </summary>
        machine_running,
        /// <summary>
        /// 换线
        /// </summary>
        workorder_change,
        /// <summary>
        /// 设备待机
        /// </summary>
        machine_idle,
        /// <summary>
        /// 设备关机
        /// </summary>
        machine_shutdown,
        /// <summary>
        /// 设备故障停机
        /// </summary>
        machine_breakdown,
        /// <summary>
        /// 设备休息
        /// </summary>
        machine_break,
        /// <summary>
        /// 设备网络中断
        /// </summary>
        network_break,
        #endregion

        //---------------------------------------------------------
        #region 异常类别
        /// <summary>
        /// 设备异常
        /// </summary>
        machine_error,
        /// <summary>
        /// 模具异常
        /// </summary>
        module_error,
        /// <summary>
        /// 设备异常错误编码
        /// </summary>
        machine_error_code,
        /// <summary>
        /// 品质呼叫
        /// </summary>
        quality_error,
        /// <summary>
        /// 物料请求
        /// </summary>
        material_require,
        /// <summary>
        /// 设备工时异常
        /// </summary>
        machine_time_error,
        #endregion

        //---------------------------------------------------------

        #region 三色灯输出
        /// <summary>
        /// 蓝灯亮起
        /// </summary>
        blue_light_on,
        /// <summary>
        /// 红灯亮起
        /// </summary>
        red_light_on,
        /// <summary>
        /// 红灯闪烁
        /// </summary>
        white_light_on,
        /// <summary>
        /// 黄灯亮起
        /// </summary>
        yellow_light_on,
        /// <summary>
        /// 绿灯输出亮起
        /// </summary>
        green_light_on,
        #endregion

        //---------------------------------------------------------

        #region 其他类别
        /// <summary>
        /// 到岗人员卡号
        /// </summary>
        andon_ack_person,
        /// <summary>
        /// 安灯异常原因确认的编码
        /// </summary>
        andon_ack_code,
        /// <summary>
        /// 安灯语音播报输出
        /// </summary>
        andon_speech_message,
        /// <summary>
        /// 机器人停机
        /// </summary>
        robot_stop,
        #endregion

        //---------------------------------------------------------

        #region ProductionSchedule
        /// <summary>
        /// 制程
        /// </summary>
        unit_no,
        /// <summary>
        /// 工单编号
        /// </summary>
        work_order,
        /// <summary>
        /// 机种信息
        /// </summary>
        part_number,
        #endregion

        #region 节拍时间
        /// <summary>
        /// cyctime
        /// </summary>
        cycle_time,
        /// <summary>
        /// 人工时间
        /// </summary>
        staff_time,
        /// <summary>
        /// 机器加工时间
        /// </summary>
        machine_time,
        /// <summary>
        /// 输送时间(z)
        /// </summary>
        convey_time
        #endregion
    }
}
