import { Ui } from './ui';
import * as ko from 'knockout';
import { StartupData } from './types';

declare const startup: StartupData;

const ui = new Ui(startup);
ko.applyBindings(ui);