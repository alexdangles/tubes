using System;
using System.Windows.Forms;
using System.Reflection;

namespace Helper
{
    public partial class MyControls : UserControl
    {
        public MyControls()
        {
            InitializeComponent();
        }

        public static T Clone<T>(T controlToClone)
        where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }
    }
}
