using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSearch.News
{
    class TNews
    {
        private String m_Url;
        private String m_Title;
        private String m_Content;
        private String m_Abstract;
        private DateTime m_Time;

        /// <summary>
        /// ����
        /// </summary>
        public String Url
        {
            get
            {
                return m_Url;
            }

            set
            {
                m_Url = value;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public String Title
        {
            get
            {
                return m_Title;
            }

            set
            {
                m_Title = value;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public String Content
        {
            get
            {
                return m_Content;
            }

            set
            {
                m_Content = value;
            }
        }

        /// <summary>
        /// ����ժҪ
        /// </summary>
        public String Abstract
        {
            get
            {
                return m_Abstract;
            }

            set
            {
                m_Abstract = value;
            }
        }


        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Time
        {
            get
            {
                return m_Time;
            }

            set
            {
                m_Time = value;
            }
        }

    }
}