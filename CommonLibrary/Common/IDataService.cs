using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// ���ݲ�ͳһ���ʼ̳нӿ�
    /// </summary>
    public interface IDataService<T>
    {
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        int Insert(T Obj);
        /// <summary>
        /// ���¶�����߸����ֶθ���
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        int Update(T obj, Dictionary<string, object> dic = null);
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns></returns>
        List<T> SelectAll();
        /// <summary>
        /// ����id���в�ѯ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T SelectByID(int id);
        /// <summary>
        /// ����id����ɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
    }
}
