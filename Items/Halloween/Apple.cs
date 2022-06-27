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
			Item.width = 20;
			Item.height = 30;
			Item.rare = -1;
			Item.maxStack = 30;
			Item.autoReuse = false;
		}
	}
}
