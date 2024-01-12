namespace DefaultNamespace
{
    public class Test
    {
        private static int level;
        private static bool[] enemyBool = new bool[4]; 

        public static void GetBinary()
        {
            for (int i = 0; i < 4; i++)
            {
                enemyBool[i] = level % 2 == 1;
                level >>= 1;
            }
        }
    }
}