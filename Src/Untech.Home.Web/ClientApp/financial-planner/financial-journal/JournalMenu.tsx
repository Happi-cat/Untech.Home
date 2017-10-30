import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { Menu } from 'semantic-ui-react';
import { ITaxonTree } from '../api';
import { pluralizeMonth } from '../../utils';

interface IJournalMenuProps {
  year: number;
  month: number;
  taxon: ITaxonTree
}

export class JournalMenu extends React.Component<IJournalMenuProps> {
  public render() {
    const parentId = this.props.taxon.parentKey;
    const upVisible = parentId != 0;

    const elements = this.props.taxon.elements || [];
    return <Menu vertical tabular>
      <Menu.Item header content='Month' />
      {[-3, -2, -1, 0, 1, 2, 3].map(diff => this.renderMonthItem(diff))}

      <Menu.Item header content='Taxon' />
      {upVisible && this.renderTaxonItem(parentId, 'Up')}
      {this.renderTaxonItem(this.props.taxon.key, this.props.taxon.name)}

      {elements.map(e => this.renderTaxonItem(e.key, e.name))}
    </Menu>;
  }

  renderMonthItem(diff: number) {
    const { year, month } = this.getMonthFromCurrent(diff);
    const isActive = diff == 0;
    const route = '/financial-planner/journal/' + year + '/' + month + '/' + this.props.taxon.key;

    return <Menu.Item key={diff} as={NavLink} active={isActive} to={route} >
      {pluralizeMonth(month) + ' - ' + year}
    </Menu.Item>;
  }

  renderTaxonItem(taxonId: number, name: string) {
    const isActive = taxonId == this.props.taxon.key;
    const { year, month } = this.props;
    const route = '/financial-planner/journal/' + year + '/' + month + '/' + taxonId;

    return <Menu.Item key={taxonId} as={NavLink} active={isActive} to={route} content={name} />;
  }

  getMonthFromCurrent(diff: number) {
    const before = diff < 0;
    let year = this.props.year;
    let month = this.props.month + diff;
    if (month < 1) {
      year -= 1;
      month += 12;
    }
    if (month > 12) {
      year += 1;
      month -= 12;
    }

    return { year, month };
  }
}

export default JournalMenu;