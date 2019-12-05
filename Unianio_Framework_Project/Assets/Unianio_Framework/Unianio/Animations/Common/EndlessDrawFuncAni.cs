using System;

namespace Unianio.Animations.Common
{
    public class EndlessDrawFuncAni : StateHolderAni
    {
        private Action<EndlessDrawFuncAni> _init;
        private Action<EndlessDrawFuncAni> _draw;

        internal EndlessDrawFuncAni()
        {
            IsGlDrawing = true;
        }
        internal EndlessDrawFuncAni Set(Action<EndlessDrawFuncAni> draw)
        {
            _draw = draw;
            return this;
        }
        internal EndlessDrawFuncAni Set(Action<EndlessDrawFuncAni> init, Action<EndlessDrawFuncAni> draw)
        {
            _init = init;
            _draw = draw;
            return this;
        }
        public override void Initialize()
        {
            if (_init != null) _init(this);
            if (_draw == null) Finish();
        }
        public override void Draw()
        {
            _draw(this);
        }
    }
}