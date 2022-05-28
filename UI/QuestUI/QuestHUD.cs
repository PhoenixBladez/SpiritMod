using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.QuestSystem;
using Terraria.UI.Chat;

namespace SpiritMod.UI.QuestUI
{
    public class QuestHUD
    {
		private bool _isActive;

		private List<QuestDisplay> _questDisplays;

		public QuestHUD()
		{
			QuestManager.OnQuestActivate += AddQuest;
			QuestManager.OnQuestDeactivate += RemoveQuest;

			_questDisplays = new List<QuestDisplay>();
			_isActive = true;
		}

		public void Toggle() => _isActive = !_isActive;
		public void AddQuest(Quest quest) => _questDisplays.Add(new QuestDisplay() { Quest = quest, IsActive = true });
		public void Clear() => _questDisplays.Clear();

		public void RemoveQuest(Quest quest)
		{
			for (int i = 0; i < _questDisplays.Count; i++)
			{
				if (_questDisplays[i].Quest.WhoAmI == quest.WhoAmI)
					_questDisplays[i].IsActive = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!_isActive) return;

			float offset = Main.mapStyle == 0 ? -Main.miniMapHeight : 0;
			float y = 365f + (Main.playerInventory ? 0 : GetInfoOffset() + offset);

			// update all the quest displays, drawing them if need be.
			for (int i = 0; i < _questDisplays.Count; i++)
			{
				if (_questDisplays[i].Update())
				{
					_questDisplays.RemoveAt(i--);
					continue;
				}

				_questDisplays[i].Draw(spriteBatch, ref y);
			}
		}

		//Offset for info accessories
		private int GetInfoOffset()
		{
			Player p = Main.LocalPlayer;
			bool[] accs = new bool[] {  p.accCalendar, p.accCritterGuide, p.accFishFinder, p.accWatch > 0, p.accOreFinder, p.accThirdEye, p.accJarOfSouls, p.accDreamCatcher, p.accStopwatch, p.accDepthMeter > 0, p.accCompass > 0 };

			int offset = 0;
			foreach (bool item in accs)
				if (item)
					offset += 24;

			return offset;
		}

		private class QuestDisplay
		{
			public Quest Quest { get; set; }
			public bool IsActive { get; set; }

			private float _opacity;
			private string _prevText;

			public bool Update()
			{
				if (IsActive)
				{
					_opacity += 0.01f;
					if (_opacity > 1f) _opacity = 1f;
				}
				else
				{
					_opacity -= 0.01f;
					if (_opacity < 0f) _opacity = 0f;
				}

				if (!IsActive && _opacity <= 0f) return true;

				return false;
			}

			public void Draw(SpriteBatch spriteBatch, ref float y)
			{
				var builder = new StringBuilder();
				Color c = QuestManager.GetCategoryInfo(Quest.QuestCategory).Color;
				string hex = $"{c.R:X2}{c.G:X2}{c.B:X2}";
				builder.Append("[c/").Append(hex).Append(":").Append(Quest.QuestName).AppendLine("]");

				if (Quest.IsActive)
				{
					string currentObjectives = Quest.GetObjectivesHUD();
					builder.Append(currentObjectives);
					_prevText = builder.ToString();
				}

				TextSnippet[] allSnippets = ChatManager.ParseMessage(_prevText, Color.White * _opacity).ToArray();
				ChatManager.ConvertNormalSnippets(allSnippets);

				string text = QuestUtils.WrapText(Main.fontMouseText, allSnippets, _prevText, 260f, 0.8f);
				string[] lines = text.Split('\n');

				float x = Main.screenWidth - (Main.playerInventory ? 450 : 280);

				float lineHeight = Main.fontMouseText.MeasureString(" ").Y * 0.8f - 1f;

				foreach (string line in lines)
				{
					TextSnippet[] snips = ChatManager.ParseMessage(line, Color.White * _opacity).ToArray();
					ChatManager.ConvertNormalSnippets(snips);

					for (int i = 0; i < snips.Length; i++) snips[i].Color *= _opacity;

					ChatManager.DrawColorCodedStringShadow(spriteBatch, Main.fontMouseText, snips, new Vector2(x, y), Color.Black * _opacity, 0f, Vector2.Zero, new Vector2(0.8f));
					ChatManager.DrawColorCodedString(spriteBatch, Main.fontMouseText, snips, new Vector2(x, y), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), out int h, -1f);

					y += lineHeight;
				}
			}
		}
	}
}
