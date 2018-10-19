using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingT.MMIS.Model.Mat
{
    /// <summary>
    /// 单据审批状态
    /// </summary>
    public enum EBillApproveState
    {
        /// <summary>
        /// 不需要审批(NotRequireApprove = 0)
        /// </summary>
        NotRequireApprove = 0,
        /// <summary>
        /// 审批未提交(UnCommitApprove = 1)
        /// </summary>
        UnCommitApprove = 1,
        /// <summary>
        /// 审批已提交(CommitApprove = 2)
        /// </summary>
        CommitApprove = 2,
        /// <summary>
        /// 审批已通过(PassApprove = 3)
        /// </summary>
        PassApprove = 3,
        /// <summary>
        /// 审批未通过(UnPassApprove = 4)
        /// </summary>
        UnPassApprove = 4,
    }

    /// <summary>
    /// 单据审核状态
    /// </summary>
    public enum EBillAuditState
    {
        /// <summary>
        /// 单据未审核(UnAuditPass=0)
        /// </summary>
        UnAuditPass = 0,

        /// <summary>
        /// 单据已审核(AuditPass=999)
        /// </summary>
        AuditPass = 999
    }

    /// <summary>
    /// 月结类型
    /// </summary>
    public enum ECarryOverType
    {
        /// <summary>
        /// 实物(RealObject = 1)
        /// </summary>
        RealObject = 1,
        /// <summary>
        /// 财务(AccObject = 2)
        /// </summary>
        AccObject = 2
    }

    /// <summary>
    /// 系统临时主单和实物主单
    /// </summary>
    public enum EBillMainType
    {
        /// <summary>
        /// 入库临时主单
        /// </summary>
        MMIS_BIN_TEMP_MAIN,
        /// <summary>
        /// 入库实物主单
        /// </summary>
        MMIS_BIN_MAIN,
        /// <summary>
        /// 出库临时主单
        /// </summary>
        MMIS_BOUT_TEMP_MAIN,
        /// <summary>
        /// 出库实物主单
        /// </summary>
        MMIS_BOUT_MAIN,
        /// <summary>
        /// 科室申领主单
        /// </summary>
        MMIS_BAPP_MAIN,
        /// <summary>
        /// 分库入库主单
        /// </summary>
        MMIS_BREC_MAIN,
        /// <summary>
        /// 盘点主单
        /// </summary>
        MMIS_BSTOCK_MAIN,
        /// <summary>
        /// 盘存报损临时主单
        /// </summary>
        MMIS_BCHK_TEMP_MAIN,
        /// <summary>
        /// 盘存报损实物主单
        /// </summary>
        MMIS_BCHK_MAIN,
        /// <summary>
        /// 调价临时主单
        /// </summary>
        MMIS_BPRICE_TEMP_MAIN,
        /// <summary>
        /// 调价实物主单
        /// </summary>
        MMIS_BPRICE_MAIN,
        /// <summary>
        /// 采购计划主单
        /// </summary>
        MMIS_BPLAN_MAIN,
        /// <summary>
        /// 封存主单
        /// </summary>
        MMIS_BLOCK,
        /// <summary>
        /// 解封单
        /// </summary>
        MMIS_BLOCK_UNDO,
        /// <summary>
        /// 消耗单
        /// </summary>
        MMIS_BUSE_MAIN,
        /// <summary>
        /// 养护单
        /// </summary>
        MMIS_BCARE_MAIN,
        /// <summary>
        /// 批次转移单
        /// </summary>
        MMIS_BSEQTRANSFER_MAIN
    }

    /// <summary>
    /// 系统财务主单
    /// </summary>
    public enum EBillAccMainType
    {
        /// <summary>
        /// 入库财务主单
        /// </summary>
        MMIS_BIN_ACC_MAIN,
        /// <summary>
        /// 出库财务主单
        /// </summary>
        MMIS_BOUT_ACC,
        /// <summary>
        /// 盘存报损财务主单
        /// </summary>
        MMIS_BCHK_ACC,
        /// <summary>
        /// 调价财务主单
        /// </summary>
        MMIS_BPRICE_MAIN_ACC,
        /// <summary>
        /// 付费单据
        /// </summary>
        MMIS_BIN_ACC_PAY
    }
}
