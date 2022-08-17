using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items;

public abstract class FoodItem : ModItem
{
	internal abstract Point Size { get; }
	internal virtual int Rarity => ItemRarityID.Blue;
	internal virtual bool Consumeable => true;

	public sealed override void SetStaticDefaults()
	{
		ItemID.Sets.IsFood[Type] = true;

		StaticDefaults();
	}

	public override sealed void SetDefaults()
	{
		Item.width = Size.X;
		Item.height = Size.Y;
		Item.rare = Rarity;
		Item.maxStack = 99;
		Item.value = Item.sellPrice(0, 0, 0, 50);
		Item.noUseGraphic = false;
		Item.useStyle = ItemUseStyleID.EatFood;
		Item.useTime = Item.useAnimation = 20;
		Item.noMelee = true;
		Item.consumable = Consumeable;
		Item.autoReuse = false;
		Item.UseSound = SoundID.Item2;
		Item.buffTime = 5 * 60 * 60;
		Item.buffType = BuffID.WellFed;

		Defaults();
	}

	public virtual void StaticDefaults() { }
	public virtual void Defaults() { }

	public sealed override bool PreDrawInInventory(SpriteBatch sb, Vector2 pos, Rectangle frm, Color drawCol, Color itemCol, Vector2 o, float scale) => FoodHelper.PreDrawInInventory(this, sb, pos, drawCol, scale);
	public sealed override bool PreDrawInWorld(SpriteBatch sb, Color light, Color a, ref float rot, ref float scale, int whoAmI) => FoodHelper.PreDrawInWorld(this, sb, light, ref rot, ref scale);
}
