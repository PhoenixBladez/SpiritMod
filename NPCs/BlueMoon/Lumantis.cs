using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
    public class Lumantis : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lumantis");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults() {
            npc.width = 22;
            npc.height = 32;
            npc.damage = 62;
            npc.defense = 20;
            npc.lifeMax = 560;
            npc.HitSound = SoundID.DD2_LightningBugHurt;
            npc.DeathSound = SoundID.NPCDeath34;
            npc.value = 760f;
            npc.knockBackResist = .2f;
            npc.aiStyle = 3;
            aiType = NPCID.WalkingAntlion;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Lumantis>()) < 3 ? .6f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 11; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 1, default(Color), .81f);
                Dust.NewDust(npc.position, npc.width, npc.height, 205, hitDirection, -1f, 1, default(Color), .71f);
            }
            if(npc.life <= 0) {
                for(int k = 0; k < 11; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 1, default(Color), .81f);
                    Dust.NewDust(npc.position, npc.width, npc.height, 205, hitDirection, -1f, 1, default(Color), .71f);
                }
            }
        }
        int timer;
        int frame;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            timer++;

            ++npc.ai[0];
			if (npc.ai[0] >= 600)
            {
                npc.velocity.X *= .0001f;
                reflectPhase = true;
                npc.defense = 9999;
            }
			else
            {
                reflectPhase = false;
                npc.defense = 20;
                if (timer >= 4)
                {
                    frame++;
                    timer = 0;
                }
                if (frame >= 3)
                {
                    frame = 0;
                }
            }
			if (npc.ai[0] >= 780)
            {
                npc.ai[0] = 0;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (!reflectPhase)
            {
                npc.frame.Y = frameHeight * frame;
            }
			else
            {
                npc.frame.Y = frameHeight * 4;
            }
        }
        bool reflectPhase;
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) {

            if (reflectPhase)
            {
                player.statLife -= item.damage;
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (reflectPhase)
            {
                projectile.hostile = true;
                projectile.friendly = false;
                projectile.penetrate = 2;
                projectile.velocity.X = projectile.velocity.X * -1f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BlueMoon/Lumantis_Glow"));
        }
    }
}
