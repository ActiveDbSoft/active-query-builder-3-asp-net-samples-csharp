import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { QuerybuilderComponent } from './querybuilder/querybuilder.component';

@NgModule({
    imports: [BrowserModule, FormsModule],
    declarations: [AppComponent, QuerybuilderComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }