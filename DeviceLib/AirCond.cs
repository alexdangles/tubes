using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Devices
{
    class AirCond
    {
        ConnectionInfo con = new ConnectionInfo("hostOrIP", 22, "username",
            new AuthenticationMethod[]{

                // Pasword based Authentication
                new PasswordAuthenticationMethod("username","password"),

                // Key Based Authentication (using keys in OpenSSH Format)
                new PrivateKeyAuthenticationMethod("username",new PrivateKeyFile[]{
                    new PrivateKeyFile(@"..\openssh.key","passphrase")
                }),
            });

        public void ExecuteShellCmd(string cmd)
        {
            using (var sshclient = new SshClient(con))
            {
                sshclient.Connect();
                Console.WriteLine(sshclient.CreateCommand(cmd).Execute());
                sshclient.Disconnect();
            }
        }

    }

}
