using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs
{
	public class DropRules
	{
		/// <summary>Wrapper for having two droprates with stacks on a Common ItemDropRule.</summary>
		public static IItemDropRule NormalvsExpertStacked(int itemID, int normal, int expert, int minStack, int maxStack)
		{
			var rule = new DropBasedOnExpertMode(ItemDropRule.Common(itemID, normal, minStack, maxStack), ItemDropRule.Common(itemID, expert, minStack, maxStack));
			return rule;
		}
	}
}
