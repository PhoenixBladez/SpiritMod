using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.UI.QuestUI;
using Terraria.UI;
using SpiritMod.Mechanics.QuestSystem;

namespace SpiritMod.UI.Elements
{
    public class UISelectableQuest : UISelectableOutlineRectPanel
	{
        public Quest MyQuest { get; protected set; }
		public UISimpleWrappableText Title { get; set; }
		public UIImageFramed Icon { get; set; }
		public List<UIImageFramed> Stars { get; protected set; }

		public UISelectableQuest(Quest quest)
		{
			Stars = new List<UIImageFramed>();
			MyQuest = quest;
		}

		public void HandleUnlock(Quest quest, bool unlocked)
		{
			if (unlocked)
			{
				Title.Text = quest.QuestName;
				Icon.Color = Color.White;
				foreach (UIImageFramed star in Stars) star.Color = Color.White;
			}
			else
			{
				StringBuilder questionMarks = new StringBuilder();
				for (int a = 0; a < quest.QuestName.Length; a++)
				{
					if (quest.QuestName[a] == ' ') questionMarks.Append(' ');
					else questionMarks.Append('?');
				}
				Title.Text = questionMarks.ToString();
				Icon.Color = Color.White * 0.5f;
				foreach (UIImageFramed star in Stars) star.Color = Color.White * 0.5f;
			}
		}
    }
}
