using NotBubbleFall.UI;
using System;

namespace NotBubbleFall
{
    public interface ISignal { }

    public static class SignalBus
    {
        public delegate void Signal<T>(object sender, T signalData) where T : ISignal;

        public static void Subscribe<T>(Signal<T> subscriber) where T : ISignal
        {
            SignalHelper<T>.Event += subscriber;
        }

        public static void Unsubscribe<T>(Signal<T> subscriber) where T : ISignal
        {
            SignalHelper<T>.Event -= subscriber;
        }

        public static void Emit<T>(object sender, T signalData) where T : ISignal
        {
            SignalHelper<T>.Emit(sender, signalData);
        }

        internal static void Subscribe<T>(ScoreLabel scoreLabel, object onScoreUpdated)
        {
            throw new NotImplementedException();
        }

        private static class SignalHelper<T> where T : ISignal
        {
            public static event Signal<T> Event;

            public static void Emit(object sender, T signalData)
            {
                Event?.Invoke(sender, signalData);
            }
        }
    }
}


