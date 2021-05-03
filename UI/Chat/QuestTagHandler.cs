using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;

using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Utilities;

namespace SpiritMod.UI.Chat
{
	public class QuestTagHandler : ITagHandler
	{
		public TextSnippet Parse(string text, Color baseColor = default, string options = null)
		{
			if (!int.TryParse(options, out int questID))
			{
				return new TextSnippet(text);
			}

			Color color = QuestUtils.GetCategoryInfo(QuestUtils.GetBaseQuestType(QuestManager.Quests[questID].QuestType)).Item1;

			return new QuestSnippet(text, color, questID);
		}

		public class QuestSnippet : TextSnippet
		{
			private int _quest;
			private bool _hovering;
			private bool _prevHover;

			public QuestSnippet(string text, Color colour, int questIndex) : base(text, colour)
			{
				_quest = questIndex;
				CheckForHover = true;
			}

			public void PostUpdate()
			{
				_prevHover = _hovering;
				_hovering = false;
			}

			public override void OnHover()
			{
				_hovering = true;
				Main.LocalPlayer.mouseInterface = true;
				if (!_prevHover)
				{
					Main.PlaySound(Terraria.ID.SoundID.MenuTick);

					_hovering = true;
				}

				if (string.IsNullOrEmpty(Main.hoverItemName))
				{
					Main.instance.MouseText("Click here to view the quest!");
					Main.mouseText = true;
				}
			}

			public override void OnClick()
			{
				SpiritMod.QuestBookUIState.SelectQuest(QuestManager.Quests[_quest]);
			}
		}
	}
}
