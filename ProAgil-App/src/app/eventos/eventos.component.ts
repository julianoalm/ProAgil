import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { templateJitUrl } from '@angular/compiler';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  registerForm: FormGroup;
  modoSalvar = 'post';

  bodyDeletarEvento = '';

  file: File;
  fileNameToUpdate: string;

  _filtroLista = '';

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService
    ) {
      this.localeService.use('pt-br');
    }

    get filtroLista(): string {
      return this._filtroLista;
    }
    set filtroLista(value: string) {
      this._filtroLista = value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista) : this.eventos;
    }

    editarEvento(evento: Evento, template: any) {
      this.modoSalvar = 'put';
      this.openModal(template);
      this.evento = Object.assign({}, evento);
      //this.fileNameToUpdate = evento.imagemURL.toString();
      //this.evento.imagemURL = '';
      this.registerForm.patchValue(this.evento);
    }

    novoEvento(template: any) {
      this.modoSalvar = 'post';
      this.openModal(template);
    }

    excluirEvento(evento: Evento, template: any) {
      this.openModal(template);
      this.evento = evento;
      this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
    }

    confirmeDelete(template: any) {
      this.eventoService.deleteEvento(this.evento.id).subscribe(
        () => {
          template.hide();
          this.getEventos();
          console.log(this.evento.id);
          //this.toastr.success('Deletado com Sucesso');
        }, error => {
          //this.toastr.error('Erro ao tentar Deletar');
          console.log(error);
        }
      );
    }

    openModal(template: any) {
      this.registerForm.reset(); // reseta o formulario no modal
      template.show();
    }

    ngOnInit(): void {
      this.validation();
      this.getEventos();
    }

    alternarImagem(): void
    {
      this.mostrarImagem = !this.mostrarImagem;
    }

    validation(): any
    {
      this.registerForm = this.fb.group({
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        imagemURL: ['', Validators.required],
        qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
      });
    }

    salvarAlteracao(template: any): any
    {
      if (this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value); //Copia todos os objetos do form para o objeto evento.

        //this.uploadImagem();

        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            template.hide();
            this.getEventos();
            console.log(novoEvento);
            //this.toastr.success('Inserido com Sucesso!');
          }, error => {
            console.log(error);
            //this.toastr.error(`Erro ao Inserir: ${error}`);
          }
        );
      } else
      {
        this.evento = Object.assign({ id: this.evento.id }, this.registerForm.value);

        //this.uploadImagem();

        this.eventoService.putEvento(this.evento).subscribe(
          () => {
            template.hide();
            this.getEventos();
            console.log(this.evento);
            //this.toastr.success('Editado com Sucesso!');
          }, error => {
            console.log(error);
            //this.toastr.error(`Erro ao Editar: ${error}`);
          }
        );
      }
    }

      filtrarEvento(filtrarPor: string): Evento[] {
        filtrarPor = filtrarPor.toLocaleLowerCase();
        return this.eventos.filter(
          evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
          );
        }

    // filtrarEvento(filtrarPor: string)
    // {
    //   filtrarPor = filtrarPor.toLocaleLowerCase()
    //   return this.eventos.filter(evento => {
    //     return evento.tema.toLocaleLowerCase().includes(filtrarPor)
    //   })
    // }

    getEventos(): void
    {
      this.eventoService.getAllEventos().subscribe(
        (_eventos: Evento[]) => {
          this.eventos = _eventos;
          this.eventosFiltrados = this.eventos;
          console.log(this.eventos);
        }, error => {
          console.log(error);
        });
    }
}
