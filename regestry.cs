using System;
using Microsoft.Win32;
using System.Windows.Forms;
// using Utility.ModifyRegistry;


namespace Program
{
    class Program
    {

        static string menuMessage = "";
        static string exepath = "";
        static string exename = "";


        static void Main(string[] args)
        {
            // var ClassesRoot = Registry.ClassesRoot;
            exepath = Console.ReadLine();
            menuMessage = Console.ReadLine();

            string[] temp = exepath.Split('\\');
            string[] temp2 = temp[temp.Length - 1].Split('.');
            exename = temp2[0];

            Console.WriteLine(exename);
            Console.WriteLine(exepath);
            Console.WriteLine(menuMessage);

            OpenFolderWith();
            OpenFileWith();
            DesktopOrElse();

        }

        // 文件夹右键菜单
        static void OpenFolderWith()
        {
            var hkCR_Dir_shell = Registry.ClassesRoot.OpenSubKey(@"Directory\shell", true);

            if (!isKeyExist(hkCR_Dir_shell))
            {
                hkCR_Dir_shell.CreateSubKey(exename).CreateSubKey("command");

            }

            Setter(hkCR_Dir_shell, "", " \"%1\"");

        }

        // 文件右键菜单
        static void OpenFileWith()
        {
            var hkCR_star_shell = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);

            // 不存在则创建
            if (!isKeyExist(hkCR_star_shell))
            {
                hkCR_star_shell.CreateSubKey(Program.exename).CreateSubKey("command");
            }

            Setter(hkCR_star_shell, ",0", " %1");

        }

        // 文件夹或桌面空白处右键菜单处
        static void DesktopOrElse()
        {
            var hkCR_Dir_Bg_shell = Registry.ClassesRoot.OpenSubKey(@"Directory\Background\shell", true);

            // 不存在则创建
            if (!isKeyExist(hkCR_Dir_Bg_shell))
            {
                hkCR_Dir_Bg_shell.CreateSubKey(Program.exename).CreateSubKey("command");
            }


            Setter(hkCR_Dir_Bg_shell, "", " \"%V\"");
        }

        // 判断键是否存在
        static bool isKeyExist(RegistryKey key)
        {

            bool isKeyExist = false;
            var subkeys = key.GetSubKeyNames();
            foreach (var i in subkeys)
            {
                if (i.Equals(Program.exename))
                {
                    isKeyExist = true;
                    break;
                }
            }
            return isKeyExist;
        }

        // 设置键值
        static void Setter(RegistryKey key, string icontail, string commandtail)
        {
            RegistryKey target_path1 = key.OpenSubKey(Program.exename, true); ;
            RegistryKey target_path2 = key.OpenSubKey(Program.exename + @"\command", true); ;


            target_path1.DeleteValue("", true); // 删除名为"(默认)"的键值

            // 设置(默认)键值，数据为 menuMessage， 类型为String(REG_SZ)
            target_path1.SetValue("", menuMessage, RegistryValueKind.String); // 设置右键菜单中的显示文字
            target_path1.SetValue("Icon", exepath+icontail, RegistryValueKind.String); // 设置右键菜单中的显示图标，一般来说就是可执行文件的路径

            target_path2.DeleteValue("", true);
            target_path2.SetValue("", exepath + commandtail, RegistryValueKind.String); // 设置可执行文件的路径
        }
    }
}
// C:\Users\是你老子我\Documents\Chris ME\Programs\C_C++ programs\start.exe