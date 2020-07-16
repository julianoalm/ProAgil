import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventosFiltrados: Evento[];
  eventos: Evento[];
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;

  _filtroLista = '';

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
    ) { }

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista) : this.eventos;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit(): void {
    this.getEventos();
  }

  alternarImagem(): void
  {
    this.mostrarImagem = !this.mostrarImagem;
  }

  filtrarEvento(filtrarPor: string): Evento[]
  {
    filtrarPor = filtrarPor.toLocaleLowerCase();

    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  getEventos(): void
  {
    this.eventoService.getAllEventos().subscribe(
      (_eventos: Evento[]) => {
        this.eventos = _eventos;
      }, error => {
        console.log(error);
    });
  }
}
