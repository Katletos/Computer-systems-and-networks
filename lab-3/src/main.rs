use actix_web::{web, App, HttpServer};

use std::sync::Mutex;

mod handlers;
use handlers::*;
mod model;
use model::*;

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    let app_state = web::Data::new(AppState {
        tickets: Mutex::new(vec![
            Ticket {
                id: 1,
                author: String::from("Jane Doe"),
            },
            Ticket {
                id: 2,
                author: String::from("Patrick Star"),
            },
        ]),
    });

    HttpServer::new(move || {
        App::new()
            .app_data(app_state.clone())
            .service(post_ticket)
            .service(get_ticket)
            .service(get_tickets)
            .service(update_ticket)
            .service(delete_ticket)
    })
    .bind(("127.0.0.1", 8080))?
    .run()
    .await
}
