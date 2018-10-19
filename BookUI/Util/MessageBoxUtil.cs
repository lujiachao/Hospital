using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BookUI.Util
{
    class MessageBoxUtil
    {
        /// <summary>
        /// 弹出提示窗口，显示普通提示信息
        /// </summary>
        /// <param name="information">普通提示信息</param>
        public static DialogResult ShowInformation(string information)
        {
            return XtraMessageBox.Show(information, "提示", MessageBoxButtons.OK,
                                                          MessageBoxIcon.Information,
                                                          MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 弹出提示窗口，显示错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public static DialogResult ShowError(string error)
        {
            return XtraMessageBox.Show(error, "错误", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Error,
                                                      MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 弹出提示窗口，显示警告信息
        /// </summary>
        /// <param name="warning">警告信息</param>
        public static DialogResult ShowWarning(string warning)
        {
            return XtraMessageBox.Show(warning, "警告", MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning,
                                                        MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 弹出询问消息框
        /// </summary>
        /// <param name="question">显示询问消息</param>
        /// <returns>对话框</returns>
        public static DialogResult ShowQuestion(string question)
        {
            return XtraMessageBox.Show(question, "询问", MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question,
                                                        MessageBoxDefaultButton.Button1);
        }
    }
}
