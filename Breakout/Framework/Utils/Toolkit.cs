#region --- Usings ---

using System;
using System.IO;
using System.Windows.Markup;
using System.Xml;

#endregion

namespace Breakout.Framework.Utils
{
    public static class Toolkit
    {
        /// <summary>
        ///     Hlavni poskytovatel nahodnych hodnot
        /// </summary>
        public static Random Random { get; } = new Random((int) DateTime.Now.Ticks);

        /// <summary>
        ///     Klonovani WPF elementu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T Clone<T>(T element)
        {
            var xaml = XamlWriter.Save(element);
            var xamlString = new StringReader(xaml);
            var xmlTextReader = new XmlTextReader(xamlString);

            return (T) XamlReader.Load(xmlTextReader);
        }
    }
}