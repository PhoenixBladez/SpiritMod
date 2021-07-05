using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Prim;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
    public class AstralGreatsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Spellblade");
            SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 60;
            item.width = 60;
            item.height = 60;
            item.useTime = 60;
            item.useAnimation = 60;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Item.sellPrice(0, 0, 1, 0);
            item.rare = ItemRarityID.Pink;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("AstralGreatswordProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => 
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "Starjinx", 10);
            recipe.AddIngredient(ItemID.FallenStar, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class AstralGreatswordProj : GreatswordProj
    {
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Astral Spellblade");

		public override void SafeSetDefaults()
        {
            chargeMax = 200; 
            minDamage = 60;
            maxDamage = 120;
            minKnockback = 4;
            maxKnockback = 9;
            chargeRate = 2;
            swingSpeed = 0.2f;
            pullBack = 0.025f;
            maxOffset = 1.5f;
        }

        public override void SafeAI()
        {
            Player player = Main.player[projectile.owner];
            if (!released && maxcharge && Main.rand.Next(5) == 0)
                Gore.NewGore(player.itemLocation - Main.rand.NextVector2Square(0, player.itemWidth).RotatedBy(player.itemRotation + MathHelper.PiOver4 * player.direction), Main.rand.NextVector2Circular(1, 1), mod.GetGoreSlot("Gores/StarjinxGore"), 1);
            if(Main.rand.Next((int)(5 - (3 * charge / chargeMax))) == 0 && charge > 60 && released)
            {
                Vector2 fakevelocity = -(projectile.position - projectile.oldPos[1]);
                fakevelocity.Normalize();
                Gore.NewGore(projectile.Center + Main.rand.NextVector2Square(-20, 20), fakevelocity.RotatedByRandom(Math.PI / 6), mod.GetGoreSlot("Gores/StarjinxGore"), 0.5f + (0.5f*charge)/chargeMax);
            }
        }

        public override void CreatePrims(Vector2 start, Vector2 mid, Vector2 end)
        {
            SpiritMod.primitives.CreateTrail(new AstralSwordPrimTrail(projectile, 
				start, 
				mid, 
				end));
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection)
        {
             if (target.boss)
                damage = (int)(damage * 1.5f);
        }
    }
}