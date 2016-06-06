using Shipwreck.CommandLine.Markup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public abstract class CommandMetadata : LoaderMetadata, ICommandMetadata, INamedMetadata
    {
        private readonly MemberNameStore _NameStore;

        #region 驕・ｻｶ隱ｭ縺ｿ霎ｼ縺ｿ繝励Ο繝代ユ繧｣

        /// <summary>
        /// <see cref="IsIgnored" />縺ｮ繝舌ャ繧ｭ繝ｳ繧ｰ繧ｹ繝医い縺ｧ縺吶・
        /// </summary>
        private bool _IsIgnored;

        /// <summary>
        /// <see cref="Order" />縺ｮ繝舌ャ繧ｭ繝ｳ繧ｰ繧ｹ繝医い縺ｧ縺吶・
        /// </summary>
        private int _Order;

        /// <summary>
        /// <see cref="Description" />縺ｮ繝舌ャ繧ｭ繝ｳ繧ｰ繧ｹ繝医い縺ｧ縺吶・
        /// </summary>
        private MarkupParagraph _Description;

        #endregion 驕・ｻｶ隱ｭ縺ｿ霎ｼ縺ｿ繝励Ο繝代ユ繧｣

        internal CommandMetadata(MemberInfo member)
        {
            _NameStore = new MemberNameStore(member.Name, member, true);
        }

        public string Name => _NameStore.Name;

        public ReadOnlyCollection<string> Names => _NameStore.Names;

        public Regex NamesPattern => _NameStore.NamesPattern;

        protected MemberInfo Member => (MemberInfo)_NameStore.Member;

        #region Load

        /// <inheritdoc />
        protected new CommandMetadata EnsureLoaded()
            => (CommandMetadata)base.EnsureLoaded();

        /// <inheritdoc />
        protected override void LoadCore()
        {
            var attr = Member.GetCustomAttribute<OptionAttribute>();

            _IsIgnored = Member.GetCustomAttribute<IgnoreAttribute>()?.IsIgnored
                            ?? attr?.IsIgnored
                            ?? false;

            _Order = Member.GetCustomAttribute<OptionOrderAttribute>()?.Order
                    ?? attr?.Order
                    ?? -1;

            _Description = Member.GetCustomAttribute<DescriptionMarkupAttribute>().Parse()
                            ?? MH.Parse(attr?.Description, attr?.DescriptionResourceType)
                            ?? Member.GetCustomAttribute<DescriptionAttribute>()?.Description.ToParagraph();
            // TODO:summary繧呈､懃ｴ｢縺吶ｋ
        }

        #endregion Load

        #region 驕・ｻｶ隱ｭ縺ｿ霎ｼ縺ｿ繝励Ο繝代ユ繧｣

        /// <summary>
        /// 繝｡繝ｳ繝舌・縺檎┌隕悶＆繧後ｋ縺九←縺・°繧堤､ｺ縺吝､繧貞叙蠕励＠縺ｾ縺吶・
        /// </summary>
        public bool IsIgnored
            => EnsureLoaded()._IsIgnored;

        /// <summary>
        /// 繝｡繝ｳ繝舌・縺ｮ蜃ｦ逅・・ｒ蜿門ｾ励＠縺ｾ縺吶・
        /// </summary>
        public int Order
            => EnsureLoaded()._Order;

        /// <summary>
        /// 繝｡繝ｳ繝舌・繧定｡ｨ縺吶・繝ｼ繧ｯ繧｢繝・・繧貞叙蠕励∪縺溘・險ｭ螳壹＠縺ｾ縺吶・
        /// </summary>
        public MarkupParagraph Description
            => EnsureLoaded()._Description;

        #endregion 驕・ｻｶ隱ｭ縺ｿ霎ｼ縺ｿ繝励Ο繝代ユ繧｣

        public abstract CommandMetadataCollection Commands { get; }

        internal abstract LoadingContextBase CreateContextForDeclaringObject(CommandMetadata metadata, LoaderSettings settings, IEnumerable<string> args, LoadingContextBase parentContext);

        object ICommandMetadata.ExecuteCore(LoadingContextBase context, object parameter)
            => ExecuteCore(context, parameter);

        internal abstract object ExecuteCore(LoadingContextBase context, object parameter);
    }
}