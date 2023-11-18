using System;

namespace Rehawk.UIFramework
{
    internal class MemberConnection : IBindingConnection
    {
        private readonly BindedMember member;
        private readonly BindingConnectionDirection direction;
        
        public event Action Changed;
            
        public MemberConnection(Func<object> getOriginFunction, string memberPath, BindingConnectionDirection direction)
        {
            member = new BindedMember(getOriginFunction, memberPath);
            this.direction = direction;
            
            member.GotDirty += OnMemberGotDirty;
        }

        public BindingConnectionDirection Direction
        {
            get { return direction; }
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
            return member.Get();
        }

        private void OnMemberGotDirty()
        {
            Changed?.Invoke();
        }
    }
}