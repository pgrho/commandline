using System;

namespace Shipwreck.CommandLine
{
    internal interface IMarkupAttribute
    {
        /// <summary>
        /// マークアップの文字列またはリソースキーを取得します。
        /// </summary>
        string MarkupOrResourceKey { get; }

        /// <summary>
        /// <see cref="ResourceType" />を含むリソースの型を取得します。
        /// </summary>
        Type ResourceType { get; }
    }
}