#region --- Usings ---

using System;
using System.Windows;

#endregion

namespace Breakout.Framework.Utils
{
    public static class MathUtils
    {
        /// <summary>
        ///     Vypocet vektoru rychlosti do daneho uhlu.
        /// </summary>
        /// <param name="angle">
        ///     Uhel ve stupnich
        /// </param>
        /// <returns>
        ///     2D normalizovany vektor rychlosti
        /// </returns>
        public static Vector CalculateVelocityVector(double angle)
        {
            // uhel se musi prevezt na radiany
            var radians = Math.PI / 180 * angle;

            var vector = new Vector
            {
                X = Math.Sin(radians),
                Y = -Math.Cos(radians)
            };

            vector.Normalize();

            return vector;
        }
    }
}