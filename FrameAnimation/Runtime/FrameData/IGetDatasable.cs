using System.Collections.Generic;

namespace VAnimation {
    public interface IGetDatasable<T> where T : FrameDataBase{
        List<T> Datas { get; }
    }
}