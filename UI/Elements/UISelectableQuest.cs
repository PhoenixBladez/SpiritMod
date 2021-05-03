using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using Terraria.UI;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.UI.QuestUI;

namespace SpiritMod.UI.Elements
{
    public class UISelectableQuest : UISelectableOutlineRectPanel
	{
		public Quest MyQuest { get; protected set; }
		public UISimpleWrappableText Title { get; set; }
		public UIImageFramed Icon { get; set; }
		public UIImageFramed Exclamation { get; set; }
		public List<UIImageFramed> Stars { get; protected set; }

		private bool _showExcl;
		private QuestBookUI _state;

		public UISelectableQuest(Quest quest, QuestBookUI state)
		{
			_state = state;
			Stars = new List<UIImageFramed>();

			DrawFilled = true;
			SelectedFillColour = new Color(102, 86, 67) * QuestBookUI.SELECTED_OPACITY;
			HoverFillColour = new Color(102, 86, 67) * QuestBookUI.HOVERED_OPACITY; 
			Height.Set(22f, 0f);
			Width.Set(0f, 1f);

			// icon
			QuestType baseType = QuestUtils.GetBaseQuestType(quest.QuestType);
			Icon = new UIImageFramed(SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/Icons"), new Rectangle(QuestUtils.Log2((int)baseType) * 18, 0, 18, 18));
			Icon.Left.Set(-10f, 0f);
			Icon.Top.Set(-10f, 0f);
			Append(Icon);

			// text
			Title = new UISimpleWrappableText(quest.QuestName, 0.7f);
			Title.Left.Set(14f, 0f);
			Title.Top.Set(-8f, 0f);
			Title.Colour = new Color(43, 28, 17);
			Append(Title);

			// difficulty stars
			float pixels = -5f;
			Texture2D starImage = SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/Star");
			for (int i = 0; i < quest.Difficulty; i++)
			{
				UIImageFramed star = new UIImageFramed(starImage, starImage.Bounds);
				star.Left.Set(pixels, 1f);
				star.Top.Set(-8f, 0f);
				pixels -= 13f;
				Append(star);
				Stars.Add(star);
			}

			Exclamation = new UIImageFramed(SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/ExclamationMark"), new Rectangle(6, 0, 3, 12));
			float pos = 20f + Title.Font.MeasureString(Title.Text).X * Title.Scale;
			Exclamation.Width.Set(3f, 0f);
			Exclamation.Height.Set(12f, 0f);
			Exclamation.Left.Set(pos, 0f);
			Exclamation.Top.Set(-7f, 0f);
			Exclamation.Color = Color.Transparent;
			Append(Exclamation);

			MyQuest = quest;
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Exclamation.Color = Color.Transparent;
			_showExcl = false;
			base.MouseOver(evt);
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);

			_state.SelectQuest(MyQuest, false);
			Exclamation.Color = Color.Transparent;
			_showExcl = false;
		}

		public override void Update(GameTime gameTime)
		{
			if (_showExcl)
			{
				float opacity = (float)(1f + Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4.5f)) * 0.5f;
				Exclamation.Color = Color.White * opacity;
			}
		}

		public void HandleState(Quest quest)
		{
			if (quest.RewardsGiven)
			{
				Title.Text = quest.QuestName;
				Exclamation.Color = Color.Transparent;
				_showExcl = false;
				Title.Colour = new Color(43, 28, 17) * 0.5f;
				Icon.Color = Color.White * 0.3f;
				foreach (UIImageFramed star in Stars) star.Color = Color.White * 0.3f;
				return;
			}

			if (!quest.IsUnlocked)
			{
				_showExcl = false;
				if (!quest.RewardsGiven)
				{
					StringBuilder questionMarks = new StringBuilder();
					for (int a = 0; a < quest.QuestName.Length; a++)
					{
						if (quest.QuestName[a] == ' ') questionMarks.Append(' ');
						else questionMarks.Append('?');
					}
					Title.Text = questionMarks.ToString();
				}
				else
				{
					Title.Text = quest.QuestName;
				}
				Exclamation.Color = Color.Transparent;
				Title.Colour = new Color(43, 28, 17) * 0.5f;
				Icon.Color = Color.White * 0.3f;
				foreach (UIImageFramed star in Stars) star.Color = Color.White * 0.3f;
				return;
			}

			Title.Text = quest.QuestName;
			Title.Colour = new Color(43, 28, 17);
			Icon.Color = Color.White;
			foreach (UIImageFramed star in Stars) star.Color = Color.White;

			if (MyQuest == _state.SelectedQuest) return;

			Exclamation.Color = Color.White;
			_showExcl = true;
		}
    }
}
