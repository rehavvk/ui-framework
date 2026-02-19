using System;

namespace Rehawk.UIFramework
{
    internal class MemberConnection : IBindingConnection
    {
        private readonly BindedMember member;

        public BindingConnectionDirection Direction { get; }

        public event Action Changed;
            
        public MemberConnection(Func<object> getOriginFunction, string memberPath, BindingConnectionDirection direction)
        {
            member = new BindedMember(getOriginFunction, memberPath);
            Direction = direction;
            
            member.GotDirty += OnMemberGotDirty;
        }

        public void Evaluate()
        {
            member.Evaluate();
        }

        public void Release()
        {
            member.Release();
        }

        public object Get()
        {
            member.TryGet(out object result);
            return result;
        }

        private void OnMemberGotDirty()
        {
            Changed?.Invoke();
        }
    }
}