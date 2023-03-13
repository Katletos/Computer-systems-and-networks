use actix_web::http::header::ContentType;

use actix_web::{delete, get, post, put, web, HttpResponse, Responder};

use super::model::*;

#[post("/tickets")]
async fn post_ticket(req: web::Json<Ticket>, data: web::Data<AppState>) -> impl Responder {
    let new_ticket = Ticket {
        id: req.id,
        author: String::from(&req.author),
    };

    let mut tickets = data.tickets.lock().unwrap();

    let response = serde_json::to_string(&new_ticket).unwrap();

    tickets.push(new_ticket);
    HttpResponse::Created()
        .content_type(ContentType::json())
        .body(response)
}

#[get("/tickets")]
async fn get_tickets(data: web::Data<AppState>) -> impl Responder {
    let tickets = data.tickets.lock().unwrap();

    let response = serde_json::to_string(&(*tickets)).unwrap();

    HttpResponse::Ok()
        .content_type(ContentType::json())
        .body(response)
}

#[get("/tickets/{id}")]
async fn get_ticket(id: web::Path<u32>, data: web::Data<AppState>) -> Result<Ticket, ErrNoId> {
    let ticket_id: u32 = *id;
    let tickets = data.tickets.lock().unwrap();

    let ticket: Vec<_> = tickets.iter().filter(|x| x.id == ticket_id).collect();

    if !ticket.is_empty() {
        Ok(Ticket {
            id: ticket[0].id,
            author: String::from(&ticket[0].author),
        })
    } else {
        let response = ErrNoId {
            id: ticket_id,
            err: String::from("ticket not found"),
        };
        Err(response)
    }
}

#[put("/tickets/{id}")]
async fn update_ticket(
    id: web::Path<u32>,
    req: web::Json<Ticket>,
    data: web::Data<AppState>,
) -> Result<HttpResponse, ErrNoId> {
    let ticket_id: u32 = *id;

    let new_ticket = Ticket {
        id: req.id,
        author: String::from(&req.author),
    };

    let mut tickets = data.tickets.lock().unwrap();

    let id_index = tickets.iter().position(|x| x.id == ticket_id);

    match id_index {
        Some(id) => {
            let response = serde_json::to_string(&new_ticket).unwrap();
            tickets[id] = new_ticket;
            Ok(HttpResponse::Ok()
                .content_type(ContentType::json())
                .body(response))
        }
        None => {
            let response = ErrNoId {
                id: ticket_id,
                err: String::from("ticket not found"),
            };
            Err(response)
        }
    }
}

#[delete("/tickets/{id}")]
async fn delete_ticket(id: web::Path<u32>, data: web::Data<AppState>) -> Result<Ticket, ErrNoId> {
    let ticket_id: u32 = *id;
    let mut tickets = data.tickets.lock().unwrap();

    let id_index = tickets.iter().position(|x| x.id == ticket_id);

    match id_index {
        Some(id) => {
            let deleted_ticket = tickets.remove(id);
            Ok(deleted_ticket)
        }
        None => {
            let response = ErrNoId {
                id: ticket_id,
                err: String::from("ticket not found"),
            };
            Err(response)
        }
    }
}
