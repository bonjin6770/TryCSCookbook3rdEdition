using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace Recipes1._1 {
    class Program {
        static void Main(string[] args) {
            // 既存のメッセージキューを開きます。
            string queuePath = @".\private$\LINQMQ";
            MessageQueue messageQueue = new MessageQueue(queuePath);
            BinaryMessageFormatter messageFormatter = new BinaryMessageFormatter();

            var query = from Message msg in messageQueue
                        // 以下の１行目でmsg.Formatterを指定することにより、
                        // Messageオブジェクトを操作できるようになります。
                        // ここでは各メッセージのインスタンスについてBinaryMessageFormatterを
                        // 指定することにより、指定した条件に一致するかどうか確認しています。
                        // その後指定したFormatterが正しく設定されたかどうかを等号により
                        // 確認しています。Where句ではboolean型の結果を返す文を
                        // 指定する必要がありますが、このようにすることでFormatterを
                        // Where句の中で指定できます。
                        where ((msg.Formatter = messageFormatter) == messageFormatter) &&
                        int.Parse(msg.Label) > 5 &&
                        msg.Body.ToString().Contains('D')
                        orderby msg.Body.ToString() descending
                        select msg;

            foreach (var msg in query) {
                Console.WriteLine("lable: " + msg.Label + " Body: " + msg.Body);
            }

        }
    }
}
