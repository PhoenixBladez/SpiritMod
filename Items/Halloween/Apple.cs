namespace SpiritMod.Items.Halloween
{
	public class Apple : CandyBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apple");
			Tooltip.SetDefault("'Who the hell gives these out?'");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = -1;
			item.maxStack = 30;
			item.autoReuse = false;
		}
	}
}
