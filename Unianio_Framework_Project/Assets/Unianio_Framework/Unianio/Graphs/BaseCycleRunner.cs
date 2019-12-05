using System;
using UnityEngine;

namespace Unianio.Graphs
{
    public abstract class BaseCycleRunner<T> where T : BaseCycleRunner<T>
    {
        Func<T, bool> _canUpdate;
        Action<T> _onInit;
        Action<T, float> _onUpdate;
        bool _isPendingInit = true;
        protected BaseCycleRunner()
        {
            ExternalCycleIndex = -1;
            InternalCycleIndex = -1;
            ExternalX01 = double.MaxValue;
            InternalX01 = double.MaxValue;

            _canUpdate = ir => true;
            _onInit = ir => { };
            _onUpdate = (ir, x) => { };

        }
        public T SetCondition(Func<T, bool> canUpdate)
        {
            _canUpdate = canUpdate ?? throw new ArgumentException("Condition function canUpdate for IntervalRunner cannot be NULL");
            return (T)this;
        }
        public T SetInit(Action<T> onInit)
        {
            _onInit = onInit ?? throw new ArgumentException("Init function onInit for IntervalRunner cannot be NULL");
            return (T)this;
        }
        public T SetUpdate(Action<T, float> onUpdate)
        {
            _onUpdate = onUpdate ?? throw new ArgumentException("Update function onUpdate for IntervalRunner cannot be NULL");
            return (T)this;
        }

        public string Label { get; protected set; }
        public int ExternalCycleIndex { get; private set; }
        public int InternalCycleIndex { get; private set; }
        public double ExternalX01 { get; private set; }
        public double InternalX01 { get; private set; }
        public double LastInternalX01 { get; private set; }
        public bool IsNewExternalCycleStart { get; private set; }

        protected float OutOfRange()
        {
            InternalX01 = -1f;
            return (float)InternalX01;
        }
        protected void SetExternal(double externalX01)
        {
            IsNewExternalCycleStart = ExternalX01 > externalX01;
            if (ExternalX01 >= externalX01)
            {
                ++ExternalCycleIndex;
            }
            ExternalX01 = externalX01;
        }
        protected float ApplyInternal(double internalX01)
        {
            if (LastInternalX01 > internalX01)
            {
                ++InternalCycleIndex;
                _isPendingInit = true;
            }
            LastInternalX01 = internalX01;

            InternalX01 = internalX01;
            if (_canUpdate((T) this))
            {
                if (_isPendingInit)
                {
                    _onInit((T) this);
                    _isPendingInit = false;
                }

                _onUpdate((T) this, (float) InternalX01);
            }
            else _isPendingInit = true;

            return (float)InternalX01;
        }
    }
}