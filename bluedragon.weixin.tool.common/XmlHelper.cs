using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace bluedragon.weixin.tool.common
{
    /// <summary>
    /// XML 辅助类
    /// </summary>
    public class XmlHelper
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {

            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }

        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(object obj)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            //去除xml声明
            // settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.UTF8;// Encoding.Default
            MemoryStream mem = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer formatter = new XmlSerializer(obj.GetType());
                formatter.Serialize(writer, obj, ns);
            }
            string strxml = Encoding.UTF8.GetString(mem.ToArray());
            return strxml.Substring(1);//去除第一个字符“”“
        }

        #endregion
    }
}
