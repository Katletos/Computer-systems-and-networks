use tokio::{
    io::{AsyncBufReadExt, AsyncReadExt, AsyncWriteExt, BufReader},
    net::TcpListener,
    sync::broadcast,
};

#[tokio::main]
async fn main() {
    let listener = TcpListener::bind("0.0.0.0:5050")
        .await
        .expect("Listener failed to bind");

    let (tx, rx) = broadcast::channel(16);

    loop {
        let (mut socket, addr) = listener.accept().await.unwrap();

        let tx = tx.clone();
        let mut rx = tx.subscribe();

        tokio::spawn(async move {
            let (reader, mut writer) = socket.split();

            let mut reader = BufReader::new(reader);
            let mut line = Vec::<u8>::new();

            loop {
                tokio::select! {
                    result = reader.read_buf(&mut line) => {
                        if result.unwrap() == 0 {
                            break;
                        }
                         tx.send((line.clone(), addr)).unwrap();
                         line.clear();
                    }
                    result = rx.recv() => {
                        let (msg, other_addr) = result.unwrap();

                        if addr != other_addr {
                            writer.write_all(&msg).await.unwrap();
                        }
                    }
                }
            }
        });
    }
}
