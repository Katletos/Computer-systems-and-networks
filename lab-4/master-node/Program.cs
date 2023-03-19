using master_node;

ServerObject server = new ServerObject("localhost", 11_000);
await server.ListenAsync();


/*
 *занят ли клиент
 *если не занят, то отправить задачу
 
    сервер отправляет
 * порядковый номер<EOM>
 * 
 * клиент отправляет <FREE><EOM>
 * выдать задачу Если получили <FREE>
 * 
 * после вычислений клиент отправляет <порядковый номер><результат><EOM>
*/

