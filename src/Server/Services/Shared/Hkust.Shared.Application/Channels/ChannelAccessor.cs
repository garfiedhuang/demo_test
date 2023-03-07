using System.Threading.Channels;

namespace Hkust.Shared.Application.Channels
{
    public class ChannelAccessor<TModel>
    {
        private static readonly Lazy<ChannelAccessor<TModel>> lazy = new(() => new ChannelAccessor<TModel>());

        private readonly ChannelWriter<TModel> _writer;
        private readonly ChannelReader<TModel> _reader;

        static ChannelAccessor()
        {
        }

        private ChannelAccessor()
        {
            var channelOptions = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            };
            var channel = Channel.CreateBounded<TModel>(channelOptions);//创建有限容量的通道 mark by garfield 20221018
            _writer = channel.Writer;
            _reader = channel.Reader;
        }

        public static ChannelAccessor<TModel> Instance => lazy.Value;

        public ChannelWriter<TModel> Writer => _writer;

        public ChannelReader<TModel> Reader => _reader;
    }
}