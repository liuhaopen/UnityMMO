
namespace LuaFramework {
    public class Protocal {
        ///BUILD TABLE
        public const int Connect = 101;     //连接服务器
        public const int Exception = 102;     //异常掉线
        public const int Disconnect = 103;     //正常断线  
		public const int MessageLine = 104;     //收到服务器消息(数据包以行做分隔)  
		public const int Message = 105;     //收到服务器消息  
    }
}