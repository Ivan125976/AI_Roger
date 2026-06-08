using System.Globalization;
using System.Text;

namespace Yocto_Roger.IO
/* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal extension I/O lib
*/
{
    /// <summary>
    /// Auxiliary class, and contains methods for work with array's
    /// </summary>
    public class Auxiliary(Parameters param)
    {
        private readonly Parameters _param = param;

        /// <summary>
        /// Преобразует в нужные типы и инициализирует данные (строки) из переданного объекта в соответствующие переменные. Если передан null, он инициализирует значения по умолчанию
        /// </summary>
        /// <param name="roger"></param>
        public void InitRogersData(MainIO.Roger? roger)
        {

            _param.passes = roger?.Passes ?? 500;
            _param.learningRate = roger?.LearingRate ?? 0.02f;
            _param.DropOutPercent = roger?.DropOutPercent ?? 3.0f;

            _param.inputNeuronsCount = roger?.InputNeuronsCount ?? 14;
            _param.middleNeuronsCount = roger?.MiddleNeuronsCount ?? 16;
            _param.outputNeuronsCount = roger?.OutputNeuronsCount ?? 8;

            _param.layers = roger?.Layers ?? 3;
        }
    }
}
