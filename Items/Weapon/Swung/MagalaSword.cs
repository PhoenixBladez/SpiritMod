using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class MagalaSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Schattenstolz");
			Tooltip.SetDefault("Inflicts Frenzy Virus \n 'I bet you don't even have Critical Draw.'\n ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 65;
            item.melee = true;
            item.width = 56;
            item.height = 56;
            item.useTime = 31;
            item.useAnimation = 31;            
            item.useStyle = 1;
            item.knockBack = 9;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
        }
        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MagalaScale", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            {
                target.AddBuff(mod.BuffType("FrenzyVirus"), 120);
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
                }
            }
        }
    }
}
